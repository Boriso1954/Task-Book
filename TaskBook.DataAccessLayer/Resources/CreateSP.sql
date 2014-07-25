
CREATE PROCEDURE spGetProjectsAndManagers
AS
BEGIN
	SELECT Projects.Id AS [ProjectID], Projects.Title, AspNetUsers.Id AS [UserID], AspNetUsers.UserName
	FROM   dbo.Projects INNER JOIN
           dbo.AspNetUsers ON dbo.Projects.Id = dbo.AspNetUsers.ProjectId INNER JOIN
           dbo.AspNetUserRoles ON dbo.AspNetUsers.Id = dbo.AspNetUserRoles.UserId INNER JOIN
           dbo.AspNetRoles ON dbo.AspNetUserRoles.RoleId = dbo.AspNetRoles.Id
WHERE      dbo.AspNetRoles.Name = N'Manager'
END