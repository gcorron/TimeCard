CREATE procedure [dbo].[uPayment] @payId int, @contractorId int, @jobId int, @hours numeric(6,2), @payDate datetime, @checkNo int, @workDay numeric(4,2)
AS


if @payId=0
begin
	insert payment (contractorId, jobId, hours, payDate, checkNo,workDay)
		values(@contractorId, @jobId, @hours, @payDate,@checkNo, @workDay)
end
else
begin
	update payment set
		contractorId = @contractorId,
		jobId = @jobId,
		hours = @hours,
		payDate = @payDate,
		checkNo = @checkNo,
		workDay=@workDay
	where payId=@payId
end