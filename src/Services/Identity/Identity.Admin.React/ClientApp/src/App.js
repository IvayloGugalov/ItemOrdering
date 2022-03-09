import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';
import CreateUser from './components/admin_panel/CreateUser';
import CreateRole from './components/admin_panel/CreateRole';
import { AdminPanel } from './components/admin_panel/AdminPanel';
import { UsersTable } from './components/admin_panel/UsersTable';

import './custom.css';
import 'bootstrap/dist/css/bootstrap.min.css';

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/counter' component={Counter} />
        <Route path='/fetch-data' component={FetchData} />
        <Route path='/add-user' component={CreateUser} />
        <Route path='/add-role' component={CreateRole} />
        <Route path='/admin-panel' component={AdminPanel} />
        <Route path='/get-users' component={UsersTable} />
      </Layout>
    );
  }
}
