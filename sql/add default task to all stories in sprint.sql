SELECT * FROM SprintStory 
JOIN Story On Story.Id = SprintStory.Story
LEft JOIN Task ON  Task.Story = Story.Id
WHERE Sprint = 2 ORDER BY Story.Id


DECLARE @myvariable varchar(50) 

DECLARE mycursor CURSOR FORWARD_ONLY
FOR SELECT Story.Id FROM SprintStory JOIN Story On Story.Id = SprintStory.Story WHERE Sprint = 2 ORDER BY Story.Id
OPEN mycursor 

WHILE (1=1)
BEGIN
FETCH NEXT FROM mycursor INTO @myvariable
IF @@fetch_status <> 0
BREAK; 

INSERT INTO Task (Guid, Omschrijving, Story, Titel)
VALUES (newid(), 'default task', @myvariable, 'default task')

END
CLOSE mycursor
DEALLOCATE mycursor
