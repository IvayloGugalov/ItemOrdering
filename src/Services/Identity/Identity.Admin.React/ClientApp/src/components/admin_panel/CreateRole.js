import React, { useState, useEffect } from 'react'
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button'
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import { variables } from '../../Variables';
import InputForm from './InputForm';


function CreateRole() {
  const [roleName, setRoleName] = useState('');
  const [description, setDescription] = useState('');
  const [accessLevel, setAccessLevel] = useState('');

  const handleSubmit = (e) => {
    e.preventDefault();
    if (roleName && description && accessLevel){
      fetch(variables.API_URL + 'admin/create-role', {
        method: 'POST',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          roleName: roleName,
          description: description,
          accessLevel: accessLevel
        })
        .then(async response =>{
          if (response.ok){
            alert("Success")
          }
          else{
            throw new Error('Network response was not ok.');
          }
        })
      })
    }
  };

  const handleAccessChange = (e) => {

  };

  return (
    <article className='justify-content-md-center' style={{ marginTop: '4rem'}} >

      <h2 style={{textAlign: 'center'}} >Creating a new role</h2>

      <Form onSubmit={handleSubmit} className='mb-6' style={{marginTop: '4rem'}}>
        <Row>
          <Col md={{span: 4, offset: 2}}>
            <InputForm property={roleName} placeHolder="Enter Role Name" type="text" handleChange={setRoleName} />
          </Col>

          <Col md={{span: 4, offset: 1}}>
            <Row>
              <p style={{textAlignment: 'center'}}>Choose role access</p>
            </Row>
            <Row>
              {/* <Form.Range onChange={handleAccessChange} /> */}

              <input type="range" ></input>

            </Row>
          </Col>
        </Row>

        <Row>
          <Col md={{span: 4, offset: 2}}>
            <InputForm property={description} placeHolder="Enter Role Description" type="text" handleChange={setDescription}/>
          </Col>          
        </Row>

      </Form>

      <Row style={{textAlign: 'center', marginTop: '4rem'}}>
        <Col md={{ span: 6, offset: 3 }} style={{justifyContent: 'center'}}>
          <Button variant="primary" type="submit">
            Submit
          </Button>
        </Col>
      </Row>
    </article>
  );
}

export default CreateRole;