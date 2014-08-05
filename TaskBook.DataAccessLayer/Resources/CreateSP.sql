
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
GO
CREATE PROCEDURE spGetUserDetailsByUserName
	@userName nvarchar(256) = NULL
AS
BEGIN
	SELECT  dbo.AspNetUsers.Id AS UserId, dbo.AspNetUsers.UserName, dbo.AspNetUsers.Email, dbo.AspNetUsers.FirstName, dbo.AspNetUsers.LastName, 
            dbo.AspNetUsers.ProjectId, dbo.Projects.Title AS ProjectTitle
	FROM    dbo.Projects RIGHT OUTER JOIN
            dbo.AspNetUsers ON dbo.Projects.Id = dbo.AspNetUsers.ProjectId
	WHERE   dbo.AspNetUsers.UserName = @userName
END