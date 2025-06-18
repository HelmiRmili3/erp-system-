$token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6ImhlbG1pQGdtYWlsLmNvbSIsImp0aSI6ImI5MWUxNDUxLWYyNDMtNDU0ZS1iZTk2LWQ0ZTg1MzIzNTg0MyIsIm5iZiI6MTc0OTgwMTU2MSwiZXhwIjoxNzQ5ODAyNDYxLCJpYXQiOjE3NDk4MDE1NjEsImlzcyI6Imh0dHA6Ly8xOTIuMTY4LjEwMC4yMzI6NTAwMCIsImF1ZCI6Imh0dHA6Ly8xOTIuMTY4LjEwMC4yMzI6NTAwMCJ9.Ds23EegnWEOi8LyImilEy9UzwK8k3f4vKRZai_5StWo"
$headers = @{
    'Authorization' = "Bearer $token"
    'Content-Type' = 'application/json'
}

try {
    $response = Invoke-RestMethod -Uri 'http://localhost:5000/api/Employees' -Method GET -Headers $headers
    $response | ConvertTo-Json -Depth 10
} catch {
    Write-Host "Error: $($_.Exception.Message)"
    Write-Host "Status Code: $($_.Exception.Response.StatusCode)"
}
