using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CIS325_Master_Web.Pages.Demos.Module3
{
    public class FlaglerCostCalculatorModel : PageModel
    {
        //============Email notification feature=============
        // === Gmail SMTP settings (simple constants for demo) ===
        private const string GMAIL_USERNAME = "flaglercisapp@gmail.com";         // TODO: change me
        private const string GMAIL_APP_PASSWORD = "chgx aejt pyto ihkk"; // TODO: change me
        private const string FROM_NAME = "Flagler Cost Calculator";

        /*
                  Go to https://myaccount.google.com/apppasswords 
                  Create an app and generate a passcode. This replaces your real password in your app.
                  Use the generated app passcode in your code.

                  Tips:
                  If you’ve set up 2-Step Verification but can’t find the option to add an app password, it might be because:
                  Your Google Account has 2-Step Verification set up only for security keys.
                  You’re logged into a work, school, or another organization account.
       */

        // This property holds the user inputs coming from the form.
        // Initialized with "new()" so it never starts out null,
        // avoiding null reference errors when the page first loads.
        [BindProperty]
        public FlaglerCostInput Input { get; set; } = new();

        // This property will store the results of the calculation.
        // It is nullable (?) because we don’t always have a result —
        // only after the form is submitted and processed.
        [BindProperty]
        public FlaglerCostResult? Result { get; set; }

        // Runs when the page is first requested (GET request).
        // At this stage, no calculations are needed — just show the empty form.
        public void OnGet() { }

        // Runs when the form is submitted (POST request).
        // public void OnPost()  --- Since we are using a third party service.
        public async Task<IActionResult> OnPostAsync()
        {
            // If validation failed (e.g., missing or invalid inputs),
            // return immediately and re-display the form with validation messages.
            if (!ModelState.IsValid) return Page();

            // Lookup tables (dictionaries) for different cost categories.
            // Key = option name (string), Value = cost (double).
            var roomRates = new Dictionary<string, double>
            {
                { "Abare", 10580 },
                { "FEC", 9890 },
                { "Ponce", 8960 },
                { "Lewis", 8960 },
                { "Cedar", 8960 }
            };

            var mealRates = new Dictionary<string, double>
            {
                { "Unlimited", 7510 },
                { "15Meal", 6000 },
                { "10Meal", 4490 },
                { "5Meal", 2290 }
            };

            var otherRates = new Dictionary<string, double>
            {
                { "Fees", 550 },
                { "Books", 1300 },
                { "Transportation", 1800 }
            };

            // At this point, model validation ensures Tuition is not null.
            // The "!" operator tells the compiler it’s safe to use .Value.
            // Need to declare Tuition as a nullable property though in the FlaglerCostInput.cs. e.g., 
            // public double? Tuition { get; set; } 
            double tuition = Input.Tuition!.Value;

            // Room and meal plan lookups:
            // TryGetValue returns true/false depending on if the key exists.
            // If not found, default to 0 to avoid errors.
            double room = roomRates.TryGetValue(Input.Room, out var r) ? r : 0;
            double meal = mealRates.TryGetValue(Input.MealPlan, out var m) ? m : 0;

            // Initialize "others" to 0, then add up matching optional expenses.
            double others = 0;
            if (Input.OtherExpenses is not null)
            {
                foreach (var exp in Input.OtherExpenses)
                {
                    if (otherRates.TryGetValue(exp, out var amt))
                        others += amt;
                }
            }

            // Scholarship is required, so use .Value safely.
            double scholarship = Input.Scholarship!.Value;

            // Total = Tuition + Room + Meal + Other expenses – Scholarship
            double total = tuition + room + meal + others - scholarship;

            // Populate the Result object with all the values.
            // This is later displayed in the Razor Page view.
            Result = new FlaglerCostResult
            {
                Tuition = tuition,
                Room = room,
                MealPlan = meal,
                Others = others,
                Scholarship = scholarship,
                Total = total
            };

            //==========Email Notification            
            if (Input.EmailResult == EmailChoice.Yes)
            {
                var html = BuildEmailHtml(Result);
                try
                {
                    await SendEmailAsync(Input.Email!, "Your Flagler Cost Estimate", html);
                    TempData["EmailStatus"] = $"Emailed results to {Input.Email}.";
                }
                catch
                {
                    TempData["EmailStatus"] = "Tried to send email, but there was an error.";
                }
            }

            return Page();

        }

        //=====Email  Helpers ====
        private static string BuildEmailHtml(FlaglerCostResult r)
        {
            var sb = new StringBuilder();
            sb.Append("<h2>Your Flagler Cost Estimate</h2>");
            sb.Append("<table style='border-collapse:collapse;font-family:Arial,sans-serif'>");
            void Row(string label, double value) =>
                sb.Append($"<tr><td style='padding:6px 12px;border:1px solid #ddd'>{label}</td><td style='padding:6px 12px;border:1px solid #ddd'>{value:C}</td></tr>");
            Row("Tuition", r.Tuition);
            Row("Room", r.Room);
            Row("Meal Plan", r.MealPlan);
            Row("Other Expenses", r.Others);
            Row("Scholarship", -r.Scholarship);
            Row("<b>Total</b>", r.Total);
            sb.Append("</table>");
            sb.Append("<p>Thank you for using the Flagler Cost Calculator.</p>");
            return sb.ToString();
        }

        //=====Call the Gmail Service======
        private static async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            using var msg = new MailMessage
            {
                From = new MailAddress(GMAIL_USERNAME, FROM_NAME),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true //enable HTML support
            };
            msg.To.Add(toEmail);

            //SMTP = Simple Mail Transfer Protocol
            using var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(GMAIL_USERNAME, GMAIL_APP_PASSWORD)
            };

            await client.SendMailAsync(msg);
        }

    }
}
