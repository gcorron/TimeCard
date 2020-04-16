create procedure sJobStart @contractorId int
as
select j.jobid, js.startday, l1.descr client, l2.descr project, l3.descr billTypeDescr
	from job j left outer join jobstart js on j.jobId=js.jobid and js.contractorId=@contractorid
		join lookup l1 on j.clientId=l1.id
		join lookup l2 on j.projectId=l2.id
		join lookup l3 on j.billType=l3.id
where j.active=1