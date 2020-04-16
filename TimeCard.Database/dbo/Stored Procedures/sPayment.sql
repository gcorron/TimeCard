create procedure sPayment @contractorId int, @jobId int
AS
select * from payment where contractorId = @contractorId and jobId=@jobId