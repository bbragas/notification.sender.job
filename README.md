# Notification Sender Job (aws lambda project)

## Build

[![Build Status](https://vegait.visualstudio.com/Notification/_apis/build/status/Application/LambdaJob/notification.sender.job?branchName=main)](https://vegait.visualstudio.com/Notification/_build/latest?definitionId=96&branchName=main)

## Visual Studio Code (Debug)

Launch Template

```json
{
    "type": "aws-sam",
    "request": "direct-invoke",
    "name": "lambda-dotnet6.0:NotificationSenderJobFunction",
    "invokeTarget": {
        "target": "template",
        "templatePath": "${workspaceFolder}/notification.sender.job/src/notification.sender.job/template.yaml",
        "logicalId": "NotificationSenderJobFunction"
    },
    "lambda": {
        "payload": {},
        "environmentVariables": {},
        "runtime": "dotnet5.0" # dont change.only accepts dotnet 5, you can run it anyway. 
    }
}
```

## AWS Toolkit Extension

> AWS Create Lambda SAM Application (SAM - Serveless Application Model)

```shell
    brew install aws-sam-cli 
    sam --version

    # print to stdout a record message
    sam local generate-event sqs receive-message

    sam local invoke NotificationSenderJobFunction --event ../../events/records.json
```
