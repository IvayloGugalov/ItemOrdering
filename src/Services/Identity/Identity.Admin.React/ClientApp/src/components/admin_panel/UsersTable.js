import React, { Component } from 'react';
import Table from 'react-bootstrap/Table'
import { variables } from '../../Variables';


export class UsersTable extends Component {
  static displayName = UsersTable.name;

  constructor(props){
    super(props);

    this.state = { users: [], loading: true };
  }

  componentDidMount() {
    this.populateUsersData();
  }

  static renderUsersTable(users){
    return (
      <Table striped bordered hover>
        <thead>
          <tr>
            <th>#</th>
            <th>First Name</th>
            <th>Last Name</th>
            <th>Email</th>
            <th>Username</th>
            <th>Roles</th>
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
            </tr>
            )}
        </tbody>
      </Table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : UsersTable.renderUsersTable(this.state.users);

    return (
      <div>
        <h1 id="tabelLabel" >Users </h1>
        <p>This component demonstrates fetching data from the server.</p>
        {contents}
      </div>
    );
  }

  async populateUsersData() {
    await fetch(variables.API_URL + 'admin/get-users', {
      method: 'GET',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
    }})
    .then(async response =>{
      if(response.ok){
        const data = await response.json();
        this.setState({ users: data, loading: false })
      }
    })
    .catch(() => this.setState({ users: [], loading: false }));
  }
}