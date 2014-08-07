
ALTER PROCEDURE spGetProjectsAndManagers
	@projectId bigint = NULL
AS
BEGIN
	IF @projectId IS NULL
		SELECT Projects.Id AS [ProjectID], Projects.Title, AspNetUsers.Id AS [UserID], AspNetUsers.UserName
		FROM   Projects INNER JOIN
			   AspNetUsers ON Projects.Id = AspNetUsers.ProjectId INNER JOIN
			   AspNetUserRoles ON AspNetUsers.Id = AspNetUserRoles.UserId INNER JOIN
			   AspNetRoles ON AspNetUserRoles.RoleId = AspNetRoles.Id
		WHERE  AspNetRoles.Name = N'Manager'
	ELSE
		SELECT Projects.Id AS [ProjectID], Projects.Title, AspNetUsers.Id AS [UserID], AspNetUsers.UserName
		FROM   Projects INNER JOIN
			   AspNetUsers ON Projects.Id = AspNetUsers.ProjectId INNER JOIN
			   AspNetUserRoles ON AspNetUsers.Id = AspNetUserRoles.UserId INNER JOIN
			   AspNetRoles ON AspNetUserRoles.RoleId = AspNetRoles.Id
		WHERE  AspNetRoles.Name = N'Manager' AND Projects.Id = @projectId
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
GO
CREATE PROCEDURE spGetUserPermissionsByUserName
	@userName nvarchar(256) = NULL
AS
BEGIN
	SELECT  dbo.Permissions.Name, dbo.Permissions.Description
	FROM    dbo.AspNetUserRoles INNER JOIN
            dbo.AspNetUsers ON dbo.AspNetUserRoles.UserId = dbo.AspNetUsers.Id INNER JOIN
            dbo.AspNetRoles ON dbo.AspNetUserRoles.RoleId = dbo.AspNetRoles.Id INNER JOIN
            dbo.Permissions INNER JOIN
            dbo.PermissionRoles ON dbo.Permissions.Id = dbo.PermissionRoles.PermissionId ON dbo.AspNetRoles.Id = dbo.PermissionRoles.RoleID
	WHERE   dbo.AspNetUsers.UserName = @userName
END