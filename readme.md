# Sharepoint.lanit-task

__Solution contains 5 main parts:__

* Configured list schema _“Site Columns\Statistics”_
* Events Listeners _“Site Columns\StatisticsEvents”_
* Service _“ISAPI\StatisticsServices”_
* Webpart _“Webparts\StatisticsDetails”_
* Webpart _“Webparts\StatisticsChart”_

__Some service modules:__

* Bootstrap module

      Provides styles and scripts for bootstrap library

* StatisticsChartModule module

      Provides styles and scripts for vuejs chart webpart

### Common
__IDE used for development:__

* Backend – VisualStudio 2015 v14.0.23
* Frontend – PhpStorm 2017.1.4

All Settings data stores in [_“App/Settings.cs”_](https://github.com/DreadfulBot/sharepoint.lanit-task/blob/master/backend/sharepoint.lanit-task/App/Settings.cs)

All Classes, Workers and Models stores in [_“App”_](https://github.com/DreadfulBot/sharepoint.lanit-task/tree/master/backend/sharepoint.lanit-task/App)

All Exceptions, Errors and Log messages writes into system events with 
provider _“SharePoint Custom Services”_

![pic](https://github.com/DreadfulBot/sharepoint.lanit-task/blob/master/readme/Screenshot_6.png)

### Site Columns\Statistics

List Contains of 3 columns: 

* __sValue__ [_Number_] - store number values typed by user in Webpart “Webparts\StatisticsDetails”
* __sDate__ [_Date and Time_] – store date and time values selected by user in 
* “Webparts\StatisticsDetails”
* __sUser__ [_Person or Group_] – store user object of author

All list fields are read only and not allowed for editing from default list view.

On _“FeatureActivated”_ event some security actions on list are happens: 

* Removing all security groups for created list
* Creating new group _“Редакторы статистики”_ and adding current user in it (user that deploying the solution)
* Assigning new group to list

__Event Receiver__ - [sharepoint.lanit_task.Features.Feature1/Feature1EventReceiver/FeatureActivated](https://github.com/DreadfulBot/sharepoint.lanit-task/blob/758c8e899225dbc49ebcae18804f3fdd982fe7ff/backend/sharepoint.lanit-task/Features/Feature1/Feature1.EventReceiver.cs#L29)

![pic](https://github.com/DreadfulBot/sharepoint.lanit-task/blob/master/readme/Screenshot_2.png)

### Site Columns\StatisticsEvents

This event listener contains 1 override method _“ItemAdding”_. During item adding, _cValue_ field value filled by user incrementing by 1 and for _cUser_ field value assigning user login name with service prefix. All other fields stay the same.

__Event Receiver__ - [sharepoint.lanit_task.StatisticsEvents/StatisticsEvents/ItemAdding](https://github.com/DreadfulBot/sharepoint.lanit-task/blob/5c0c61523001495838dc0458e7bba59894959c23/backend/sharepoint.lanit-task/Site%20Columns/StatisticsEvents/StatisticsEvents.cs#L26)

### ISAPI\StatisticsServices

Made by this instructions - [link](https://social.technet.microsoft.com/wiki/contents/articles/24194.sharepoint-2013-create-a-custom-wcf-rest-service-hosted-in-sharepoint-and-deployed-in-a-wsp.aspx). I’m not sure this is a best practice. Service Host manifest file contains a _PublicKeyToken_ key-value pair and, if you have any errors with it, you need to generate it by yourself. Follow this manual – [link](https://blogs.msmvps.com/windsor/2011/11/04/walkthrough-creating-a-custom-asp-net-asmx-web-service-in-sharepoint-2010/).

Service implements 3 Operation Contracts:

* _HelloWorld_ – for tests
* _GetUserStatistics/{id}_ – returning statistics of user with ID = _{id}_
* _GetCurrentUserStatistics_ – deprecated. Returning current logged in user statistics and requires user to be authenticated.

All Get Methods are allowed by link having mask:
```
http://site/_vti_bin/StatisticsServices.svc/{method}/{params}
```

![pic](https://github.com/DreadfulBot/sharepoint.lanit-task/blob/master/readme/Screenshot_3.png)

## Attention:

For using services, Anonymous Access must be turned on in SharePoint WebSite.
Run IE as admin, go to SharePoint Administration Center, to ``_admin/WebApplicationList.aspx``, select Application on 80 port and follow instructions on screenshots:

![pic](https://github.com/DreadfulBot/sharepoint.lanit-task/blob/master/readme/Screenshot_8.png)

__Code Behind__ - [sharepoint.lanit_task.Services/StatisticsServices](https://github.com/DreadfulBot/sharepoint.lanit-task/blob/5c0c61523001495838dc0458e7bba59894959c23/backend/sharepoint.lanit-task/ISAPI/StatisticsServices.svc.cs#L11)

### Webpart “Webparts\StatisticsDetails”

![pic](https://github.com/DreadfulBot/sharepoint.lanit-task/blob/master/readme/Screenshot_4.png)

Visual webpart. It is made like a form with 2 input fields – Value and Date. All fields have hidden Required and CompareByType Validators. Bootstrap was used for faster decoration.

After adding item, page refreshes with status code and message, associated for this code.

Command for creating new list item runs with elevated privileges.

![pic](https://github.com/DreadfulBot/sharepoint.lanit-task/blob/master/readme/Screenshot_5.png)

__Code Behind__ - [sharepoint.lanit_task.Webparts.StatisticsDetails/ StatisticsDetailsUserControl/btnSave_Click](https://github.com/DreadfulBot/sharepoint.lanit-task/blob/5c0c61523001495838dc0458e7bba59894959c23/backend/sharepoint.lanit-task/Webparts/StatisticsDetails/StatisticsDetailsUserControl.ascx.cs#L19)

### Webpart “Webparts\StatisticsChart”

Visual webpart. Contains page with vuejs application on it, that gets data for current user from StatisticsService and display it as a chart. .ascx page contains link to .css and .js files of frontend project, that deploy to SharePoint as a module. <div id=”app”></div> is an entry point for vuejs component.

__TODO__: for versioning, need to find a solution to include dynamically generated .css and .js files by their names into .ascx. At current time, you need to refresh page with `ctrl + f5` after deploying to download new .css and .js.

![pic](https://github.com/DreadfulBot/sharepoint.lanit-task/blob/master/readme/Screenshot_7.png)

### Frontend

Statistics chart is based on [vue-chartjs](https://github.com/apertureless/vue-chartjs). 

_“src/App.vue”_ is a root component, where my StatisticsChart component including with _backendUrl_ property (url to _/__vti_bin/StatisticsService.svc/GetUserStatistics_). This property contains a link from where script will receive data by get http request. For http requests [axios](https://github.com/axios/axios) lib were used.

Script starts from mounted stage:

* Getting _userId_ from _ _spPageContextInfo_ object
* Running async function _loadStatistics_
* After _loadStatistics_ execution, calling parent vue-chartjs method and passing data for display in it.

__Source Code__ of my custom component - [src/components/StatisticsChart.js](https://github.com/DreadfulBot/sharepoint.lanit-task/blob/master/frontend/statistics-chart/src/components/StatisticsChart.js) 

#### Running Frontend Project

For running Frontend project:

* Install [nodejs](https://nodejs.org/en/) 
* Install [yarn](https://yarnpkg.com/lang/en/) or use internal nodejs npm
* Install [python 3](https://www.python.org/downloads/windows/)
* In console, go to [frontend/statistics-chart](https://github.com/DreadfulBot/sharepoint.lanit-task/tree/master/frontend/statistics-chart) folder
* Run command `yarn install/npm install` for downloading dependencies
* Run command `yarn f/npm run f` for building module in folder _“build”_
* Place all content of _“build”_ folder into _“VSProject/StatisticsChartModule”_ __(clear it before)__
* Reload VS Project

#### Some comments:

For faster working, [vue-cli](https://github.com/vuejs/vue-cli) was used. And it was first mistake – some compiled files have symbol _“\~”_ in names, that is now allowed by VS and SharePoint, where “~” is a special character. Modification of vue-cli source code was a bad idea and I made a python script called _“prepare-module.py”_. This script renames all files how I need and generates Elements.xml and SharePointProjectItem.spdata for simple import into VS Project as a module.

In next time, I will use my custom webpack scripts, like here - [link](https://github.com/DreadfulBot/vuejs.quiz)

To run this script, run command `yarn m/npm run m` in cmd, or directly python command `py -3 prepare-module.py` after building project.