import React, { useState, useRef } from 'react'
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button'
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import InputForm from './InputForm';
import { variables } from '../../Variables';
import useAxiosPrivate from '../../custom-hooks/useAxiosPrivate'

function CreateRole() {
  const [roleName, setRoleName] = useState('');
  const [description, setDescription] = useState('');
  const [accessLevel, setAccessLevel] = useState('0');
  const [accessLevelInfo, setAccessLevelInfo] = useState('Low access level');
  const [errorMsg, setErrorMsg] = useState('');

  const errorRef = useRef();
  const axiosPrivate = useAxiosPrivate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (roleName && description && accessLevel){
      try {
        const response = await axiosPrivate.post(variables.IDENTITY_API_URL + 'admin/create-role',
          JSON.stringify({
            roleName: roleName,
            description: description,
            accessLevel: accessLevel}
            ),
          {
            headers: { 'Accept': 'application/json' },
            withCredentials: true
          }
        );
      } catch (err) {
        if (!err?.response) {
            setErrorMsg('Error, no response');
        } else {
            setErrorMsg(`Adding a new role failed ${err}`);
        }
      }    
    };
  }

  const handleRoleLevel = (e) => {
    setAccessLevel(e);
    switch (e) {
      case "0":
        setAccessLevelInfo("Low access level");
        break;
      case "1":
        setAccessLevelInfo("Normal access level");
        break;        
      case "2":
        setAccessLevelInfo("Privileged access level");
        break;
      case "3":
        setAccessLevelInfo("Administrator access level");
        break;
      default:
        console.log('default');
    }
  };

  return (
    <article className='justify-content-md-center' style={{ marginTop: '4rem'}} >

      <h2 style={{textAlign: 'center'}} >Creating a new role</h2>

      <Form onSubmit={handleSubmit} className='mb-6' style={{marginTop: '4rem'}}>
        <Row>
          <Col md={{span: 4, offset: 2}}>
            <InputForm property={roleName} placeHolder="Enter Role Name" type="text" handleChange={setRoleName} />
          </Col>

          <Col md={{span: 4}}>
            <Row className='ml-3'>
              <p style={{textAlignment: 'center'}}>Choose role access</p>
            </Row>
          </Col>
        </Row>

        <Row>
          <Col md={{span: 4, offset: 2}}>
            <InputForm property={description} placeHolder="Enter Role Description" type="text" handleChange={setDescription}/>
          </Col>  
          <Col md={{span: 4}} className='ml-3'>
            <div>
              <input
                  type="range"
                  value={accessLevel}
                  class="form-range"
                  min="0"
                  max="3"
                  step="1"
                  id="roleLevelRange"
                  onChange={(e) => handleRoleLevel(e.target.value)} />
            </div>

            <div>
              <p>{accessLevelInfo}</p>
            </div>
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

      <p ref={errorRef} className={errorMsg
               ? "errmsg"
               : "offscreen"}>
                   {errorMsg}
               </p>
    </article>
  );
}

export default CreateRole;