USE [TaxiManagementSystem]
GO

DECLARE @username VARCHAR(30) ='yourusername';

Update Users 
Set IsOwner = 1
where UserName = @username



Delete from Driver Where [UserId] in (Select * from Users Where UserName = @username);


INSERT INTO [dbo].[Owner]
           ([LastName]
           ,[FirstName]
           ,[Email]
           ,[Phonenumber]
           ,[UserId])
     VALUES
           ((Select TOP 1(LastName) from Users Where UserName = @username)
           ,(Select TOP 1(FirstName) from Users Where UserName = @username)
           ,(Select TOP 1(Email) from Users Where UserName = @username)
           ,04000000
           ,(Select TOP 1(UserId) from Users Where UserName = @username));


