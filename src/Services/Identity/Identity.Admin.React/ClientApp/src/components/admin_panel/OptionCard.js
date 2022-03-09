import React, { Component } from 'react';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import Card from 'react-bootstrap/Card'

export class OptionCard extends Component {
  static displayName = OptionCard.name;

  constructor(props){
    super(props);
    console.log(this.props.onClick)
  }

  render() {

    return(
      <Row className="rounded-lg border border-primary">
        <Col xs={6} sm={4} md={4} >
          <Card.Img variant="top" src={this.props.image} />
        </Col>
        <Col>
          <Card
            border="0"
            style={{ width: '20rem', cursor: "pointer" }}
            onClick={this.props.onClick}>
            <Card.Body>
              <Card.Title>{this.props.title}</Card.Title>
              <Card.Text>
                {this.props.description}
              </Card.Text>
            </Card.Body>
          </Card>
        </Col>
      </Row>
    );
  }
}