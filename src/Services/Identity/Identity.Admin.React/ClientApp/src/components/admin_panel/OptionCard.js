import React from 'react';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import Card from 'react-bootstrap/Card'

const OptionCard = ({title, description, image, onClick}) => {
  return(
    <Row className="rounded-lg border border-primary">
      <Col xs={6} sm={5} md={4} >
        <p>Image here</p>
        <Card.Img variant="top" src={image} />
      </Col>
      <Col>
        <Card
          border="0"
          style={{  cursor: "pointer" }}
          onClick={onClick}>
          <Card.Body>
            <Card.Title>{title}</Card.Title>
            <Card.Text>
              {description}
            </Card.Text>
          </Card.Body>
        </Card>
      </Col>
    </Row>
  );
}

export default OptionCard;