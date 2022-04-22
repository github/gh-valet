# GitHub Valet CLI

[![.github/workflows/ci.yml](https://github.com/github/gh-valet/actions/workflows/ci.yml/badge.svg)](https://github.com/github/gh-valet/actions/workflows/ci.yml)[![.github/workflows/ci.yml](https://github.com/github/gh-valet/actions/workflows/ci.yml/badge.svg)](https://github.com/github/gh-valet/actions/workflows/ci.yml)

Valet is a tool to help facilitate migrations to GitHub Actions. Valet supports migrating pipelines from Jenkins, Azure DevOps, GitLab CI, Circle CI, and Travis CI to GitHub Actions.

This repository provides functionality that extends the [GitHub CLI](https://cli.github.com/) to migrate pipelines to GitHub Actions using Valet.

> Valet is in a private preview and customers must be onboarded prior to using the Valet Issue Ops workflow. Please reach out to GitHub Sales to enquire about getting into the private preview.

## Supported platforms

Valet current supports migrating pipelines to GitHub Actions from the following platforms:

- Azure DevOps
- Jenkins
- Travis CI
- Circle CI
- GitLab CI

Learn more about how Valet works for each of the supported platforms in the documentation [here](https://github.com/valet-customers/distribution/blob/main/README.md).

> Support can be requested by creating an issue in this repository. We'll be available to provide support Monday through Friday between the hours of 9 AM EST and 5 PM PST.

## Getting started with the Valet CLI

First, you'll need to download the official [GitHub CLI](https://cli.github.com).

Next, the Valet CLI extension can be installed via this command:

```bash
$ gh extension install github/gh-valet
```

To verify the extension is installed, run this command:

```bash
$ gh valet -h


```


### Command line
To get started you'll need to download the official [GitHub CLI](https://cli.github.com). You can run `gh extension install github/gh-gei` to install the GEI CLI. Once installed, run `gh gei --help` to learn about the options.

```
gh-gei
  CLI for GitHub Enterprise Importer.

Usage:
  gh-gei [options] [command]

Options:
  --version       Show version information
  -?, -h, --help  Show help and usage information

Commands:
  generate-script       Generates a migration script. This provides you the ability to review the steps that this tool will take, and optionally modify the script if desired before running it.
  grant-migrator-role   Allows an organization admin to grant a USER or TEAM the migrator role for a single GitHub organization. The migrator role allows the role assignee to perform migrations into the target organization.
                        Note: Expects GH_PAT env variable to be set.
  migrate-repo          Invokes the GitHub APIs to migrate the repo and all repo data.
  revoke-migrator-role  Allows an organization admin to revoke the migrator role for a USER or TEAM for a single GitHub organization. This will remove their ability to run a migration into the target organization.
                        Note: Expects GH_PAT env variable to be set.
```

To generate a migration script, you'll need to set `GH_PAT` as an environment variable for your destination and `GH_SOURCE_PAT` for your source location. 

## GitHub Enterprise Server (GHES) to GitHub Migration Usage

General usage will use the `generate-script` command to create a script that can be used to migrate all repositories from a GHES organization.

Due to many GHES instances containing firewall rules restricting the direct API access that GEI needs to perform a migration, the GEI CLI uploads the migration's archive data to Azure Blob Storage as a step prior to starting the migration using GEI. This allows GEI to perform a migration without directly accessing the GHES instance. To get started you'll need to setup an [Azure storage account](https://docs.microsoft.com/en-us/azure/storage/common/storage-account-create?tabs=azure-portal). Then, you'll pass in a [connection string](https://docs.microsoft.com/en-us/azure/data-explorer/kusto/api/connection-strings/storage-connection-strings) to the CLI for that Azure storage account, specified in the parameters below. GEI CLI will take care of the rest.

### Command Line

Generating a migration script for GHES is very similar to GitHub to GitHub migration scripts, but requires a couple extra parameters. You can run `gh gei generate-script --help` to learn about all of the options.

The extra options you will need when migrating from GHES are:
- `--ghes-api-url` - (Required) The api endpoint for the hostname of your GHES instance. For example: http(s)://api.myghes.com"
- `--azure-storage-connection-string` - (Required) The connection string for the Azure storage account, used to upload data archives pre-migration. For example: \"DefaultEndpointsProtocol=https;AccountName=myaccount;AccountKey=mykey;EndpointSuffix=core.windows.net\". It's recommend to use quotes around this value since the connection string itself uses characters that might otherwise be interpreted by bash.
- `--no-ssl-verify` (Optional) Disables SSL verification when communicating with your GHES instance. All other migration steps will continue to verify SSL. If your GHES instance has a self-signed SSL certificate then setting this flag will allow data to be extracted.

Here's an example:
```
gh gei generate-script --github-source-org "source-ghes-org" --github-target-org "target-ghec-org" --ghes-api-url "https://api.myghesinstance.com" --azure-storage-connection-string "DefaultEndpointsProtocol=https;AccountName=myazureaccount;AccountKey=myazurestoragekey;EndpointSuffix=core.windows.net" --no-ssl-verify
```

This will generate a new file (named `./migrate.ps1` by default) which gives you an opportunity to review the steps GEI-CLI will take. Once ready, running the `./migrate.ps1` file will kick off the migration!

After running the migration and validating the migration data, you will need to remove the archive data from your Azure storage account. If you're done with all of your migrations, the blob container(s) containing the archives can be deleted. 

## Azure DevOps to GitHub Migration Usage

Execute the executable without any parameters to learn about the options. General usage will use the `generate-script` option to create a script that can be used to migrate all repositories from an Azure DevOps org and re-wire Azure Boards and Azure Pipelines connections.

### Command line
```
ado2gh
  Migrates Azure DevOps repos to GitHub
Usage:
  ado2gh [options] [command]

Options:
  --version       Show version information
  -?, -h, --help  Show help and usage information

Commands:
  generate-script
  rewire-pipeline
  integrate-boards
  share-service-connection
  disable-ado-repo
  lock-ado-repo             Makes the ADO repo read-only for all users. It does this by adding Deny permissions for the Project Valid Users group on the repo.
  configure-auto-link
  create-team               Creates a GitHub team and optionally links it to an IdP group.
  add-team-to-repo
  migrate-repo
```

To generate a script, you'll need to set an `ADO_PAT` as an environment variable. Performing any of the commands that touch GitHub will need the `GH_PAT` environment variable.

### Running a Migration 

Covering running a migration from **Azure DevOps** to **GitHub.com**. 

#### Download and Configure the GH-GEI command-line Tool

Navigate to the `Releases` for this repository and grab the latest release for your local operating system. Note: ado2gh is for Azure DevOps -> GitHub migrations, gei is for GitHub -> GitHub migrations.
![Releases](https://user-images.githubusercontent.com/29484535/145065021-35f37a00-1a25-42a4-804d-11fd9f8cc811.png)
Once you have downloaded the `Release`, you need to extract it to your local machine.
**Note** you will want to place the `octoshift` executable somewhere easy to reference or add to your path.
![Folder View](https://user-images.githubusercontent.com/29484535/145065026-a519a7f0-fc1d-46a1-a1a5-cd96743b1bd1.png)

* [Linux add folder to path](https://linuxize.com/post/how-to-add-directory-to-path-in-linux/)
* [Powershell add folder to path](https://stackoverflow.com/questions/714877/setting-windows-powershell-environment-variables/714918)

Once you have pathed the tooling, you will need to set `2` environment variables. 

* One will be called `GH_PAT` and will be your **GitHub Personal Access Token**
* The other will be called `ADO_PAT` and will be your **Azure DevOps Access Token** 

The scope needed for each token will depend on what command(s) you want run. See the scenarios below to ensure you have properly scoped personal access tokens. It's recommended that you pick the scenario which fits the needs of everything you want to do as part of migrating. 

#### I just want to run some migrations and grant/revoke the migrator role
Create a GitHub and Azure DevOps personal access tokens with the scope defined in GEI's [documentation](https://docs.github.com/en/early-access/github/migrating-with-github-enterprise-importer/migrating-to-github-enterprise-cloud-with-the-importer#step-2-assign-the-migration-permissions-role-and-ensure-the-migrator-has-the-expected-pat-scopes). This will allow you to run these commands:

* generate-script (--repos-only)
* migrate-repo
* grant/revoke-migrator-role
* create-team
* add-team-to-repo
* configure-autolink

#### I want to do the above and also lock & disable the repository being migrated from Azure DevOps
In order to use the following pre & post migration commands:

* lock-ado-repo
* disable-ado-repo
* generate script (without the --repos-only flag)

You will need to include these additional scopes for your Azure DevOps personal access token in addition to the ones listed in GEI's [documentation](https://docs.github.com/en/early-access/github/migrating-with-github-enterprise-importer/migrating-to-github-enterprise-cloud-with-the-importer#step-2-assign-the-migration-permissions-role-and-ensure-the-migrator-has-the-expected-pat-scopes):

* `Service Connection (Read)`
* `Build (Read & execute)`
* `Security (Manage)`
* `Code (Read, write, and manage)`

#### I want to do the above and also re-connect Azure Pipelines & Boards to the newly migrated repository on GitHub
If you want to re-connect an Azure Pipline or Board to the migrated repo then you'll need your ADO personal access token to be `full access`.

![image](https://user-images.githubusercontent.com/40493721/145903240-6a6d04cd-ba03-47f4-84aa-6af741a8ddd6.png)

At this point, you can now begin to run and test the import process.

#### Run the Migrations

Once you have configured the `octoshift`(*gh-gei*) command-line tool and `environment variables` for the person access tokens, you can run the command-line tool and see all available options.
![octoshift help](https://user-images.githubusercontent.com/29484535/145065029-ea8b3fcd-fcea-4f9b-ba7e-f9d3407e17fa.png)

The first step you will want to run is `generate-script` to help outline all commands to migrate an entire **Azure Project**
This command will generate a `migrate.ps1` file in the local folder. You will want to open it with an editor tool as it can be quite large. It's recommended that you include the `--repos-only` flag the first time. 
Tools like `Atom`, `VSCode`, or `NotePad++` are great ways to see the data.

You can then use this as a guide to pick and choose which commands you would like to run.

## Contributions

See [Contributing](CONTRIBUTING.md) for more info on how to get involved.