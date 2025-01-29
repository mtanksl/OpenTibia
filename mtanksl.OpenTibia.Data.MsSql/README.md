# Adding migration

dotnet ef migrations add Initial -- Server=localhost;Database=mtots;Trusted_Connection=True;

# Scripting migration

dotnet ef migrations script -- Server=localhost;Database=mtots;Trusted_Connection=True;