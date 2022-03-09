import React, { Component } from 'react';
import Button from 'react-bootstrap/Button'
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import { Container } from 'react-bootstrap';
import { OptionCard } from './OptionCard';
import { Link } from 'react-router-dom';



export class AdminPanel extends Component {
  static displayName = AdminPanel.name;

  constructor(props){
    super(props);
  }

  render(){
    return (
      <Container>
        <h1>Admin panel</h1>

        <Row style={{marginTop:'12rem'}}>
          <Col md={5}>
            <Link to="/add-user" style={{ textDecoration: 'none'}}>
              <OptionCard
                title='Add users'
                description='Creating new users is easy with just two steps.'
                image='' />
            </Link>
          </Col>

          <Col md={{ span: 5, offset: 2 }}>
            <Link to="/get-users" style={{ textDecoration: 'none'}}>
              <OptionCard
                title='Check all users'
                description='View all users'
                image='' />
            </Link>
          </Col>
        </Row>

        <Row style={{marginTop:'5rem'}}>
          <Col  md={5}>
            <Link to="/add-role" style={{ textDecoration: 'none'}}>
              <OptionCard
                title='Create a new role'
                description='Creating new role for your users.'
                image='' />
            </Link>
          </Col>
        </Row>
      </Container>
    );
  }
}