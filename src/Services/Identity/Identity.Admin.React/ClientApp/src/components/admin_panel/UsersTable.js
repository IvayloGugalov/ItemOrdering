import React, { useEffect, useState, useRef } from 'react';
import Table from 'react-bootstrap/Table';
import Button from 'react-bootstrap/Button'
import { Container } from 'react-bootstrap';
import { variables } from '../../Variables';
import useAxiosPrivate from '../../custom-hooks/useAxiosPrivate';

const UsersTable = () => {
  const [users, setUsers] = useState();
  const [errorMsg, setErrorMsg] = useState('');
  const [isLoading, setIsLoading] = useState(true);
  const axiosPrivate = useAxiosPrivate();

  const errorRef = useRef();

  useEffect(() => {
    const getUsers = async () => {
      try {
        const response = await axiosPrivate.get(variables.IDENTITY_API_URL + 'admin/get-users',
        {
          headers: { 'Content-Type': 'application/json' },
          withCredentials: true
        });

        setUsers(response?.data);
      } catch (err) {
        if (!err.response) {
          setErrorMsg(`Error, no response ${err}`);
        } else {
            setErrorMsg(err.response);
        }
        errorRef.current.focus();

        console.error(err);
        setUsers([]);
      }
      setIsLoading(false);
    }

    getUsers();
  }, []);

  const handleDeleteUser = async (email) => {
    try {
      const response = await axiosPrivate.post(variables.IDENTITY_API_URL + 'admin/delete-user', 
        JSON.stringify({email}),
        {
          headers: {'Content-Type': 'application/json'},
          withCredentials: true
      });
      
      console.log(response?.data);
      if (response?.status === 200) {
        let refreshedUsers = users.filter((user) => user.email != email);
        setUsers(refreshedUsers);
      }

    } catch (err) {
      if (!err.response) {
        setErrorMsg(`Error, no response: ${err}`);
      } else {
        setErrorMsg(`Not able to delete user ${email}. ${err.response.data}`);
      }
    }
  }

  if (isLoading) {
    return (
      <div>
        <h1>Loading...</h1>
      </div>
    );
  } else {
    return (
      <article>
        {users?.length
        ? (
            <Container>
              <Table striped bordered hover>
              <thead>
                <tr>
                  <th>#</th>
                  <th>First Name</th>
                  <th>Last Name</th>
                  <th>Email</th>
                  <th>Username</th>
                  <th>Roles</th>
                  <th></th>
                </tr>
              </thead>
              <tbody>
                {users.map(user =>
                  <tr key={user.email}>
                    <td></td>
                    <td>{user.firstName}</td>
                    <td>{user.lastName}</td>
                    <td>{user.email}</td>
                    <td>{user.userName}</td>
                    <td>{user.roles}</td>
                    <div style={{marginLeft: '1rem', marginTop: '5px'}}>
                      <Button variant="outline-danger" onClick={() => handleDeleteUser(user.email)}>Delete</Button>
                    </div>
                  </tr>
                  )}
              </tbody>
            </Table>
            <p ref={errorRef} className={errorMsg
               ? "errmsg"
               : "offscreen"}>
                   {errorMsg}
               </p>
            </Container>
          )
        : <p>No users to display</p>
        }
      </article>
    );
  }
}

export default UsersTable;