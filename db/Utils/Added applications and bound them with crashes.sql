insert into application
	([key], language)
values
	('WebWeaver', 'CS'),
	('WebService', 'CS'),
	('ChargebeeWebhook', 'CS'),
	('WebWeaver.Client', 'JS')

update c
set c.applicationId = a.id
from crash c
inner join application a on a.[key] = c.module

update p
set p.applicationId = a.id
from problem p
inner join application a on a.[key] = p.module