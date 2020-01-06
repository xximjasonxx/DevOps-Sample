# DevOps Automation Example Project
*This project does not represent a complete solution. It is designed to show example and concepts that can be used in your project. Copy and Paste only when appropriate and use this for reference purposes only*

## General Methodology
All builds leverage containers and ephemeral infrastructure for environments between Dev and Production. This is done to save cost and demonstrate how Infrastructure as Code can be valuable in the development process.

Further, I attempted to do as much as possible using Terraform scripts such that the process for standing up this pipeline was minimal and required only a few commands to make happen, including Secret management

To achieve this, you will find folders which contain Terraform scripts that are global, environmental, and app specific in nature. This was done to show the boundary of service management since we want to persist certain resources between runs (Insights, App Service plans, etc).

Global is run before everything to ensure any drift at the Global level is corrected. Within each Stage we run the our environment scripts, these scripts contain a parameter to identify which environment is being targeted. This stands up or corrects drift within Environment specific services (ex. Event Grid). Finally, we call our App specific scripts. In the case of these app resoruces, for Test and Metric, we destroy them at the end of life. We maintain the Insights pieces so we can review the data that was generated during their usage.

The goal is to show how we MIGHT organize a multi-service architecture in future projects and how we might pass data created such that our scripts enable true environments.

### Infrastructure vs Code Changes
Each build script is setup to only listen for changes within their relevant directories, all Infrastructure folders are stored at the root level under the **infrastructure** folder. This is to ensure that infrastructure changes can be made **WITHOUT** going through a complete build process where there are no code changes.

This is achieved by using two artifact types within the Release pipeline: Source and Build. Build is commonly seen and will kick the release pipeline off automatically when there is a code change that results in a succuessful build. The Source type is seen less often but enables us to start the Release process manually when tweaking infrastructure - the Release will then use the latest build when creating the release.

## AuthApi
AuthApi serves as a simple JWT broker service issueing tokens to connecting users. Authenticated requests to other services must use this JWT token to be allowed to call endpoints

### Understanding the build pipeline
For this build we use a multi-job approach to divide and conquer validating and building our artifact. One job is responsible for building the "unchecked" image, pushing it our Azure Container Registry, and then validating it against Anchore (disabled).

The other job runs the unit tests and would do various other checks against the source code (Dependency Scanning, OSS scanning, unit test validation) to validate that what is being built into the image in the other Job is valid. Only when both jobs complete with success is the next stage allowed to execute

The final stage rebuilds the image with a "checked" tag to indicate the image has passed inspection, it is pushed to the same Azure Container Registry. Additionally, any other artifacts needed by the Release process (except Infrastructure scripts) are "published" so they can be referenced in the Release pipeline

## UserApi
UserApi serves general User level data such as Name (Username is duplicated with the Auth data source)

## Setup

#### Backend State