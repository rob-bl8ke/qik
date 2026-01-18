## Using a Persistent HTTPS Dev Certificate in a .NET Dev Container

### 1. Export and Store the Certificate

- On your host machine, export your trusted ASP.NET dev certificate:
  ```sh
  dotnet dev-certs https -ep "${HOME}/.aspnet/https/aspnetapp.pfx" -p "SecurePwdGoesHere"
  ```
- Now navigate to `${HOME}/.aspnet/https` and you should see your certificate there.
- Copy (using drag n drop) the exported aspnetapp.pfx file into your project (from your Windows host machine) at `docs/cert/aspnetapp.pfx` (overwrite the existing one in source control).

### 2. Configure devcontainer.json

- In your devcontainer.json, set up the environment and automate certificate placement:
  ```json
  "remoteEnv": {
    "ASPNETCORE_Kestrel__Certificates__Default__Password": "SecurePwdGoesHere",
    "ASPNETCORE_Kestrel__Certificates__Default__Path": "$HOME/.aspnet/https/aspnetapp.pfx"
  },
  "portsAttributes": {
    "7212": {
      "protocol": "https"
    }
  },
  "postCreateCommand": "mkdir -p $HOME/.aspnet/https && cp /workspaces/qik/docs/cert/aspnetapp.pfx $HOME/.aspnet/https/"
  ```
- This ensures the certificate is copied to the correct location every time the container is rebuilt. Otherwise, the certificate will be lost when the container rebuilds.

### 3. Rebuild the Dev Container

- Use the VS Code command palette:  
  **Dev Containers: Rebuild Container**
- The `postCreateCommand` will copy the certificate into `$HOME/.aspnet/https/` inside the container.

### 4. Run and Verify

- Start your ASP.NET app as usual.
- It will use HTTPS on port 7212, with the certificate and password you provided.

To ensure that your certificate is where it should be and that it has not expired.

```bash
cd ~/.aspnet/https
ls -la

# Check the expiry date
openssl pkcs12 -in /home/vscode/.aspnet/https/aspnetapp.pfx -clcerts -nokeys | openssl x509 -noout -enddate
```

---

This setup ensures your dev certificate is always available, even after container rebuilds, and avoids manual steps each time.

Here’s how to create a long-lived, trusted self-signed certificate for ASP.NET development on Windows 11, and use it in your Dev Container:

---

### 1. Create a Self-Signed Certificate (10 years)

Open **PowerShell as Administrator** and run:

```powershell
$cert = New-SelfSignedCertificate -DnsName "localhost" -CertStoreLocation "cert:\CurrentUser\My" -NotAfter (Get-Date).AddYears(10) -FriendlyName "ASP.NET Dev Cert"
```

This creates a certificate valid for 10 years in your personal certificate store.

---

### 2. Export the Certificate as .pfx

Still in PowerShell, run:

```powershell
$pwd = ConvertTo-SecureString -String "SecurePwdGoesHere" -Force -AsPlainText
Export-PfxCertificate -Cert $cert -FilePath "$env:USERPROFILE\.aspnet\https\aspnetapp.pfx" -Password $pwd
```

---

### 3. Trust the Certificate

- The certificate is already in your personal store, so it’s trusted by Windows.
- To trust it for browsers like Chrome/Edge, you may need to import it into the “Trusted Root Certification Authorities” store:
  1. Open `certmgr.msc`
  2. Find your “ASP.NET Dev Cert” under “Personal > Certificates”
  3. Right-click > All Tasks > Export (choose .cer format, no private key)
  4. Import the .cer file into “Trusted Root Certification Authorities > Certificates”

---

### 4. Use the Certificate in Your Dev Container

- Copy the exported `aspnetapp.pfx` to your project (e.g., aspnetapp.pfx)
- Commit it to source control if you want persistence.
- Your `devcontainer.json` and `postCreateCommand` setup will copy it into the container as before.

---

### 5. Do I Need to Remove the Old Trusted Certificate?

- If you already have a trusted certificate, you don’t need to remove it, but you should update your container and ASP.NET app to use the new long-lived one.
- You can remove old certificates from `certmgr.msc` if you want to avoid confusion.
