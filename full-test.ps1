# Step 1: Login and get token
Write-Host "Step 1: Logging in..."
$loginBody = @{
    email = "helmi@gmail.com"
    password = "Password-123"
} | ConvertTo-Json

try {
    $loginResponse = Invoke-RestMethod -Uri 'http://localhost:5000/api/Auth/login' -Method POST -ContentType 'application/json' -Body $loginBody
    Write-Host "Login successful!"
    Write-Host "Token: $($loginResponse.data.token.Substring(0,50))..."
    
    # Step 2: Use token to access employees
    Write-Host "`nStep 2: Accessing employees endpoint..."
    $headers = @{
        'Authorization' = "Bearer $($loginResponse.data.token)"
        'Content-Type' = 'application/json'
    }
    
    $employeesResponse = Invoke-RestMethod -Uri 'http://localhost:5000/api/Employees' -Method GET -Headers $headers
    Write-Host "Employees retrieved successfully!"
    $employeesResponse | ConvertTo-Json -Depth 10
    
} catch {
    Write-Host "Error: $($_.Exception.Message)"
    if ($_.Exception.Response) {
        Write-Host "Status Code: $($_.Exception.Response.StatusCode)"
    }
}
