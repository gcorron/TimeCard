﻿CREATE procedure [dbo].[sWork] @contractorId int, @workDay numeric(6,2), @payCycle bit
AS
-- exec sWork 0,0,1
select w.workId, w.contractorId, w.jobId, w.workDay, w.descr,w.hours
	,l1.descr contractor
	,j.descr job
from work w
	join lookup l1 on w.contractorId=l1.id
	join job j on w.jobId=j.jobId
where (contractorId=@contractorId or @contractorId = 0)
and (workday=@workDay or (floor(workday)=floor(@workDay) and @payCycle=1))