CREATE procedure [dbo].[sJobTimeCardUnpaidCycles] @contractorId int, @jobId int
as
-- exec sJobTimeCardUnpaidCycles 13,34
select floor(workDay) workDay, sum(hours) hours
from work
where jobId=@jobId
and not exists(
	select * from payment where contractorid=@contractorId and jobId=@jobId and workDay=floor(workDay))
group by floor(workDay)