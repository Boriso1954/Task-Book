
CREATE PROCEDURE spGetProjectsAndManagers
	@projectId bigint = NULL
AS
BEGIN
	IF @projectId IS NULL
		SELECT Projects.Id AS [ProjectID], Projects.Title, AspNetUsers.Id AS [UserID], AspNetUsers.UserName
		FROM   Projects INNER JOIN
			   AspNetUsers ON Projects.Id = AspNetUsers.ProjectId INNER JOIN
			   AspNetUserRoles ON AspNetUsers.Id = AspNetUserRoles.UserId INNER JOIN
			   AspNetRoles ON AspNetUserRoles.RoleId = AspNetRoles.Id
		WHERE  AspNetRoles.Name = N'Manager' AND Projects.DeletedDate IS NULL
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
	SELECT  AspNetUsers.Id AS UserId, AspNetUsers.UserName, AspNetUsers.Email, AspNetUsers.FirstName, AspNetUsers.LastName, 
            AspNetRoles.Name AS Role, Projects.Id AS ProjectId, Projects.Title AS ProjectTitle
	FROM    Projects RIGHT OUTER JOIN
            AspNetUsers ON dbo.Projects.Id = AspNetUsers.ProjectId INNER JOIN
            AspNetUserRoles ON AspNetUsers.Id = AspNetUserRoles.UserId INNER JOIN
            AspNetRoles ON AspNetUserRoles.RoleId = AspNetRoles.Id
	WHERE   dbo.AspNetUsers.UserName = @userName
END
GO
CREATE PROCEDURE spGetUserPermissionsByUserName /* TODO Obsolete*/
	@userName nvarchar(256) = NULL
AS
BEGIN
	SELECT  Permissions.Name, Permissions.Description
	FROM    AspNetUserRoles INNER JOIN
            AspNetUsers ON AspNetUserRoles.UserId = AspNetUsers.Id INNER JOIN
            AspNetRoles ON AspNetUserRoles.RoleId = AspNetRoles.Id INNER JOIN
            Permissions INNER JOIN
            PermissionRoles ON Permissions.Id = PermissionRoles.PermissionId ON AspNetRoles.Id = PermissionRoles.RoleID
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