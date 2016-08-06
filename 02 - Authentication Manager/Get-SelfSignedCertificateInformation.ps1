<#
.SYNOPSIS
Gets metadata for a certificate (cer)

.DESCRIPTION


.EXAMPLE
PS C:\> .\Get-SelfSignedCertificateInformation.ps1 -CertPath "c:\MyCert.cer"

#>
Param(
   [Parameter(Mandatory=$true, HelpMessage="Certificate path (.cer)")]
   [string]$CertPath
)

$cert = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2
$cert.Import($CertPath)
$rawCert = $cert.GetRawCertData()
$base64Cert = [System.Convert]::ToBase64String($rawCert)
$rawCertHash = $cert.GetCertHash()
$base64CertHash = [System.Convert]::ToBase64String($rawCertHash)
$KeyId = [System.Guid]::NewGuid().ToString()

$keyCredentials = 
'"keyCredentials": [
    {
      "customKeyIdentifier": "'+ $base64CertHash + '",
      "keyId": "' + $KeyId + '",
      "type": "AsymmetricX509Cert",
      "usage": "Verify",
      "value":  "' + $base64Cert + '"
     }
  ],'
$keyCredentials

Write-Host "Certificate Thumbprint:" $cert.Thumbprint