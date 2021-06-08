# Message API Server
This Project is a simple API server which stores messages and users in an associated SQLite Database.

## Authentication
All requests made to the server need to be made using basic authentication.

## Endpoints
### Users
#### Create A User
Endpoint: /api/CreateUser
Parameters:
* username
* password
* name

Returns the user just created

#### Find A User
Method: GET
Endpoint: /api/Users
Returns all users found in the database

#### Get User Details Based On Users
Method: GET
Endpoint: /api/Users/<username>
Returns the User object

#### Update Your User Profile
Method: PUT
Endpoint: /api/Users/<username>
Parameters:
* username
* updated User object

#### Delete Your Profile
Method: DELETE
Endpoint: /api/Users/<username>
Parameters:
* username

#### Get All Sent Messages
Method: GET
Endpoint: /api/Users/<username>/sent-messages/
Returns a list of message objects

#### Get All Received Messages
Method: GET
Endpoint: /api/Users/<username>/received-messages/
Returns a list of message objects

### Messages
#### Get A Specific Message
Method: GET
Endpoint: /api/Messages/<id>
Returns an the object of the message

#### Update A Specific Message
Method: PUT
Endpoint: /api/Messages/<id>
Parameters:
* id
* updated Message Object
Returns the updated Message Object

#### Create A New Message
Method: POST
Endpoint: /api/Messages/
Parameters:
* Message Object
Returns the newly created messages

#### Delete A Message
Method: DELETE
Endpoint: /api/Messages/<id>

### User Creation
Method: POST
Endpoint: /api/CreateUser
Parameters:
* username
* password
* name
