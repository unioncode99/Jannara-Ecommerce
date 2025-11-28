# Database Reset Workflow

-To rebuild the database and load the latest schema, run this command from the repo folder:

  - sqlcmd -S localhost -E -i reset.sql
- This will drop the existing **Jannara** database (if it exists) and recreate it using the latest `schema.sql` in the project.

   
