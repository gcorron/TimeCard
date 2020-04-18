CREATE procedure [sJobs]
as
select jobId id, descr
from job
order by descr