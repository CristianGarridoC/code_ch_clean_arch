# Code Challenge

Create a CRUD REST API application for:

Product: Name, Brand, Price

    Use Dapper ORM
  
    Clean architecture

Extra points:

    Use cache
  
    Throttling requests
  
    Money pattern

Commands to execute the project:

    cd source/API
    docker compose build
    docker compose up

Notes:
    
    - The product controller has a rate limit of 3 requests in a time window of 10 seconds if more than 3 requests are made within those 10 seconds it returns a 429 error.
    - The GetAll endpoint uses a cache with an expiration time of 20 seconds.
    - If a product is added, updated or removed, it will not be reflected in the GetAll endpoint until the cache expires.