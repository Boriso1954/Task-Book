CREATE PROCEDURE spGetProjectsAndManagers
	@projectId bigint = NULL
AS
BEGIN
	IF @projectId IS NULL
		SELECT	Projects.Id AS ProjectId, Projects.Title, AspNetUsers.Id AS UserId, AspNetUsers.UserName
		FROM	AspNetUsers INNER JOIN
				ProjectUsers ON AspNetUsers.Id = ProjectUsers.UserId INNER JOIN
				Projects ON ProjectUsers.ProjectId = Projects.Id INNER JOIN
				AspNetUserRoles ON AspNetUsers.Id = AspNetUserRoles.UserId INNER JOIN
				AspNetRoles ON AspNetUserRoles.RoleId = AspNetRoles.Id
		WHERE	AspNetRoles.Name = N'Project Manager' AND Projects.DeletedDate IS NULL
	ELSE
		SELECT	Projects.Id AS ProjectId, Projects.Title, AspNetUsers.Id AS UserId, AspNetUsers.UserName
		FROM	AspNetUsers INNER JOIN
				ProjectUsers ON AspNetUsers.Id = ProjectUsers.UserId INNER JOIN
				Projects ON ProjectUsers.ProjectId = Projects.Id INNER JOIN
				AspNetUserRoles ON AspNetUsers.Id = AspNetUserRoles.UserId INNER JOIN
				AspNetRoles ON AspNetUserRoles.RoleId = AspNetRoles.Id
		WHERE	AspNetRoles.Name = N'Project Manager' AND Projects.DeletedDate IS NULL AND Projects.Id = @projectId
END
GO
CREATE PROCEDURE spGetUserDetailsByUserName
	@userName nvarchar(256) = NULL
AS
BEGIN
	SELECT	AspNetUsers.Id AS UserId, AspNetUsers.UserName, AspNetUsers.FirstName, AspNetUsers.LastName, AspNetRoles.Name AS Role, 
			Projects.Id AS ProjectId, Projects.Title AS ProjectTitle, AspNetUsers.Email
	FROM	AspNetUsers INNER JOIN
			AspNetUserRoles ON AspNetUsers.Id = AspNetUserRoles.UserId INNER JOIN
			AspNetRoles ON AspNetUserRoles.RoleId = AspNetRoles.Id LEFT OUTER JOIN
			ProjectUsers ON AspNetUsers.Id = ProjectUsers.UserId LEFT OUTER JOIN
			Projects ON ProjectUsers.ProjectId = Projects.Id
	WHERE   AspNetUsers.UserName = @userName
END
GO
CREATE PROCEDURE spGetPermissionsByRole
	@roleName nvarchar(256) = NULL
AS
BEGIN
	SELECT  Permissions.Name, Permissions.Description
	FROM    Permissions INNER JOIN
            PermissionRoles ON Permissions.Id = PermissionRoles.PermissionId INNER JOIN
            AspNetRoles ON PermissionRoles.RoleID = AspNetRoles.Id
	WHERE   AspNetRoles.Name = @roleName
END