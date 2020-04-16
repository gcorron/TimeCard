CREATE procedure [dbo].[sPaymentSummary] @contractorId int, @workDay numeric(6,2)
as
-- exec sPaymentSummary 13,2
;with hourSummary (jobId, totalHours)
as (
select w.jobId, sum(hours)
from work w join job j on w.jobid=j.jobid
where contractorid=@contractorid and j.active=1 and workDay<@workDay
group by w.jobId
)

select j.jobid, l1.descr client, l2.descr project, l3.descr billtype, isnull(h.totalHours,0) billed, isnull(sum(p.hours),0) paid
from job j
	join lookup l1 on j.clientid=l1.id
	join lookup l2 on j.projectid=l2.id
	join lookup l3 on j.billtype=l3.id
	left outer join hourSummary h on j.jobId=h.jobId
	left outer join payment p on j.jobid=p.jobId
where j.active=1 and not (h.totalHours is null)
group by j.jobid, l1.descr, l2.descr, l3.descr,h.totalHours