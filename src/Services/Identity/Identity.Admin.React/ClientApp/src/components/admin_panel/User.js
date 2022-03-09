import React, { Component } from 'react';


export class User extends Component {
  static displayName = User.name;

  constructor(props){
    super(props);

    this.state = { firstName: '', lastName: '', email: '', userName:'', roles: []};
  }

}