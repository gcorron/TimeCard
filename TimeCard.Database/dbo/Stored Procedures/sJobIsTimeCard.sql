


CREATE procedure sJobIsTimeCard @jobId int
as
-- exec sJobIsTimeCard 34
select cast(case when lu.descr='TC' then 1 else 0 end as bit) isTimeCard
from job j join lookup lu on j.billType=lu.id
where jobid=@jobid