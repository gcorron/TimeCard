create procedure uJobStart @contractorId int, @jobId int, @startDay numeric(6,2)
as
update jobStart set startDay=@startDay
where contractorId=@contractorId and jobId=@jobId

if @@ROWCOUNT=0
begin
	insert jobStart (contractorId, jobId, startDay)
		values (@contractorId, @jobId, @startDay)
end