# Asana
Looking at ways of keeping the Asana Tasks updated based on Pull Requests, we have created some service hooks to be configured inside Azure DevOps.  
For this to work, keep in mind we have to follow a standard in our PR's titles like this suggested on the last line of this template: [Pull Request template](https://dev.azure.com/thinklogicinc/Thinklogic/_git/Thinklogic?version=GBmaster&path=/.azuredevops/pull_request_template.md)

Below you can see a list of possible updates to be made to a task.

# Custom Fields updates

## Pull request created

### Trigger Link
- Trigger on this type of event: **Pull request created**

### Action Link
- URL: `https://thinklogicintegrationfunctions.azurewebsites.net/api/UpdateAsanaTaskCustomField?code=IZKWLj65wC2c1SUMQ9q72dW45DaZjRR0thG24j2KfsQL0aWesOoffA==`
- HTTP headers:  
`asana-project-path:resource.title`  
`asana-task-path:resource.title`  
`asana-custom-field-key:Pull Request link`  
`asana-custom-field-value:resource._links.web.href`  

### Trigger Status
- Trigger on this type of event: **Pull request created**

### Action Status
- URL: `https://thinklogicintegrationfunctions.azurewebsites.net/api/UpdateAsanaTaskCustomField?code=IZKWLj65wC2c1SUMQ9q72dW45DaZjRR0thG24j2KfsQL0aWesOoffA==`
- HTTP headers:  
`asana-project-path:resource.title`  
`asana-task-path:resource.title`  
`asana-custom-field-key:Pull Request status`  
`asana-custom-field-value:Created`  

## Pull request waiting for the author

### Trigger
- Trigger on this type of event: **Pull request updated**

### Action
- URL: `https://thinklogicintegrationfunctions.azurewebsites.net/api/UpdateAsanaTaskCustomField?code=IZKWLj65wC2c1SUMQ9q72dW45DaZjRR0thG24j2KfsQL0aWesOoffA==`
- HTTP headers:  
`asana-project-path:resource.title`  
`asana-task-path:resource.title`  
`filter-path:message.text`  
`filter-value:is waiting for the author`  
`asana-custom-field-key:Pull Request status`  
`asana-custom-field-value:Waiting for the author` 

## Pull request approved

### Trigger
- Trigger on this type of event: **Pull request updated**

### Action
- URL: `https://thinklogicintegrationfunctions.azurewebsites.net/api/UpdateAsanaTaskCustomField?code=IZKWLj65wC2c1SUMQ9q72dW45DaZjRR0thG24j2KfsQL0aWesOoffA==`
- HTTP headers:  
`asana-project-path:resource.title`  
`asana-task-path:resource.title`  
`filter-path:message.text`  
`filter-value:approved`  
`asana-custom-field-key:Pull Request status`  
`asana-custom-field-value:Approved` 

## Pull request completed

### Trigger
- Trigger on this type of event: **Pull request updated**
- Change: **Status changed**

### Action
- URL: `https://thinklogicintegrationfunctions.azurewebsites.net/api/UpdateAsanaTaskCustomField?code=IZKWLj65wC2c1SUMQ9q72dW45DaZjRR0thG24j2KfsQL0aWesOoffA==`
- HTTP headers:  
`asana-project-path:resource.title`  
`asana-task-path:resource.title`   
`filter-path:message.text`  
`filter-value:completed`  
`asana-custom-field-key:Pull Request status`  
`asana-custom-field-value:Completed` 
