CREATE procedure [dbo].[sWorkExtended] @contractorId int, @workDay numeric(6,2), @payCycle bit, @payCycles int
AS
-- exec sWorkExtended 13,33,1

declare @lastCycle int = floor(@workDay)
declare @firstCycle int=@lastCycle - @payCycles + 1

select w.workId, w.contractorId, w.jobId, w.workDay, w.descr,w.hours
	,l1.descr contractor
	,j.descr job
	,j.clientId, j.projectId
	,l2.descr client, l2.val clientCode, l3.descr project, l4.descr billType, l5.descr workType
from work w
	join lookup l1 on w.contractorId=l1.id
	join job j on w.jobId=j.jobId
	join lookup l2 on j.clientId=l2.id
	join lookup l3 on j.projectId=l3.id
	join lookup l4 on j.billType=l4.id
	join lookup l5 on w.workType=l5.id
where (contractorId=@contractorId or @contractorId = 0)
and (workday=@workDay or (floor(workday) between @firstCycle and @lastCycle and @payCycle=1))
order by workDay,workid