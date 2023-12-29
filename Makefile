path :=$(if $(path), $(path), "./")
awsRegion:="us-east-1"

.PHONY: help
help: ## - Show this help message
	@printf "\033[32m\xE2\x9c\x93 usage: make [target]\n\n\033[0m"
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | sort | awk 'BEGIN {FS = ":.*?## "}; {printf "\033[36m%-30s\033[0m %s\n", $$1, $$2}'

.PHONY: build-common
build-common: ## - execute build common tasks
	@ dotnet clean notification.sender.job.sln
	@ dotnet restore notification.sender.job.sln

.PHONY: build
build: build-common ## - build a debug assembly to the current platform (windows, linux or darwin(mac))
	@ dotnet build --no-restore notification.sender.job.sln
	@ ls -lah $(path)/src/notification.sender.job/bin/Debug/net6.0/

.PHONY: build-release
build-release: build-common ## - build a release assembly to the current platform (windows, linux or darwin(mac))
	@ dotnet build -c Release --no-restore notification.sender.job.sln
	@ ls -lah $(path)/src/notification.sender.job/bin/Release/net6.0/

.PHONY: test
test: build-release ## - build a release assembly to the current platform (windows, linux or darwin(mac))
	@ ls tests/*Tests*/*.csproj | xargs -L1 dotnet test --logger:trx

.PHONY: scan-sec
scan-sec:
	@ dotnet tool update -g security-scan
	@ security-scan notification.sender.job.sln --export=notification.sarif
	@ echo ""
	@ @ cat $(path)/notification.sarif | jq -r '(["LEVEL","LINE","FILE","RULE-ID","MESSAGE"] | (., map(length*"-"))), (.runs[].results[] | [.level, .locations[].physicalLocation.region.startLine, .locations[].physicalLocation.artifactLocation.uri, .ruleId, .message.text] ) | @tsv' | column -t 

.PHONY: lint
lint: ## - check lint against dotnet projects
	@ dotnet tool update -g dotnet-format
	@ dotnet format --no-restore --verbosity detailed

.PHONY: publish
publish: ## - build a release assembly to the current platform (windows, linux or darwin(mac))
	@ rm -rf $(path)/src/notification.sender.job/publish
	@ dotnet publish -c Release $(path)/src/notification.sender.job/notification.sender.job.csproj -o $(path)/src/notification.sender.job/publish
	@ rm -f $(path)/src/notification.sender.job/publish/*.pdb $(path)/src/notification.sender.job/publish/appsettings.Development.json
	@ ls -lah $(path)/src/notification.sender.job/publish

.PHONY: docker-build
docker-build: ## - build docker image
	@ cd $(path)/src/notification.sender.job && docker build -f Dockerfile -t $(imageName):$(version) .

docker-push:
	@ docker tag "$(imageName)":"$(version)" 434885180636.dkr.ecr."$(awsRegion)".amazonaws.com/"$(imageName)":"$(version)"
	@ docker push 434885180636.dkr.ecr."$(awsRegion)".amazonaws.com/"$(imageName)":"$(version)"

.PHONY: docker-scan
docker-scan: ## - Scan for known vulnerabilities
	@ printf "\033[32m\xE2\x9c\x93 Scan for known vulnerabilities the smallest and secured golang docker image based on scratch\n\033[0m"
	@ cd $(path)/src/notification.sender.job &&  docker scan -f Dockerfile notification-sender

.PHONY: sonar-scan
sonar-scan: ## - start sonar qube locally with docker (you will need docker installed in your machine)
	@ # login with user: admin pwd: notification
	@ $(SHELL) _scripts/sonar-start.sh

.PHONY: sonar-stop
sonar-stop: ## - stop sonar qube docker container
	@ docker stop sonarqube

.PHONY: publish-s3
publish-s3:
	@ aws s3 cp ${path}/${imageVersion}.zip s3://vegait-lambda-code-storage/${project}/$(app)/${imageVersion} 