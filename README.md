# Cloudflare Turnstile Integration in ASP.NET 8 MVC

This project demonstrates how to integrate Cloudflare Turnstile (Invisible CAPTCHA) in an ASP.NET 8 MVC application. It provides a simple login form with Turnstile protection.

## Prerequisites

- .NET 8 SDK
- A Cloudflare account with Turnstile configured
- Turnstile Site Key and Secret Key

## Quick Start

1. Clone the repository:
   ```bash
   git clone https://github.com/ponggun/poc-cloudflare-turnstile.git
   cd poc-cloudflare-turnstile
   ```

2. Set your Turnstile keys:

   a. Using environment variables:
   ```bash
   # Windows PowerShell
   $env:TURNSTILE_SITE_KEY="YOUR_SITE_KEY"
   $env:TURNSTILE_SECRET="YOUR_SECRET_KEY"

   # Linux/macOS
   export TURNSTILE_SITE_KEY="YOUR_SITE_KEY"
   export TURNSTILE_SECRET="YOUR_SECRET_KEY"
   ```

   b. Or in `src/TurnstilePoC/appsettings.Development.json`:
   ```json
   {
     "Turnstile": {
       "SiteKey": "YOUR_SITE_KEY_HERE",
       "SecretKey": "YOUR_SECRET_KEY_HERE"
     }
   }
   ```

3. Run the application:
   ```bash
   # Using VS Code task (recommended)
   # Press Cmd+Shift+B (macOS) or Ctrl+Shift+B (Windows/Linux) and select "Run ASP.NET 8 MVC App"
   # Or run manually:
   cd src/TurnstilePoC
   dotnet run

   # Then Ngrok
   ngrok http https://localhost:5001 --host-header=rewrite
   ```

4. Open https://localhost:5001/Account/Login in your browser

## Using Docker

1. Build the Docker image:
   ```bash
   cd src/TurnstilePoC
   docker build -t turnstile-poc .
   ```

2. Run the container:
   ```bash
   docker run -p 8080:8080 -p 8081:8081 \
   -e TURNSTILE_SITE_KEY="YOUR_SITE_KEY" \
   -e TURNSTILE_SECRET="YOUR_SECRET_KEY" \
   turnstile-poc
   ```

3. Access the application at http://localhost:8080/Account/Login

## Creating Your Turnstile Widget

1. Go to the [Cloudflare dashboard](https://dash.cloudflare.com/)
2. Navigate to "Security" > "Turnstile"
3. Click "Add Site" and follow the instructions
4. Choose "Invisible" widget type for the best user experience
5. Copy the Site Key and Secret Key

## How It Works

1. The login page includes the Cloudflare Turnstile widget script
2. When a user submits the form, Turnstile performs invisible validation
3. The server validates the token by sending it to Cloudflare's verification API
4. The verification response is logged and the user is authenticated or shown an error

## Troubleshooting

- Check the application logs for detailed Turnstile verification responses
- Ensure your Site Key and Secret Key are correctly set
- Make sure your domain is allowed in the Turnstile configuration on Cloudflare

## License

This project is licensed under the MIT License - see the LICENSE file for details.