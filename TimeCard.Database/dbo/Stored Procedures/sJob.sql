CREATE procedure sJob
as
select jobId id, descr
from job
order by descr