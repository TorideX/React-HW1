import React, { useEffect, useState } from "react";
import {
  Button,
  Modal,
  ModalHeader,
  ModalBody,
  ModalFooter,
  Form,
  FormGroup,
  Label,
  Input,
} from "reactstrap";

export const EditDeveloperModal = ({developer, isModalOpen, toggleModal, submitCallback}) => {

  const [firstName, setFirstName] = useState()
  const [lastName, setLastName] = useState()
  const [age, setAge] = useState()

  useEffect(()=>{
    setFirstName(developer.firstName)
    setLastName(developer.lastName)
    setAge(developer.age)
    console.log(developer)
  }, [developer])


  const firstNameOnChange = (e) =>
  {
    setFirstName(e.target.value)
  }
  const lastNameOnChange = (e) =>
  {
    setLastName(e.target.value)
  }
  const ageOnChange = (e) =>
  {
    setAge(e.target.value)
  }

  return (
    <div>
      <Modal isOpen={isModalOpen} toggle={toggleModal}>
        <ModalHeader toggle={toggleModal}>Add Developer Page</ModalHeader>
        <Form onSubmit={submitCallback}>
          <ModalBody>
            <Input id="id" name="id" type="hidden" value={developer.id}/>
            <FormGroup className="mb-4">
              <Label for="firstName">First Name</Label>
              <Input id="firstName" name="firstName" placeholder="First Name" onChange={firstNameOnChange} value={firstName}/>
            </FormGroup>
            <FormGroup className="mb-4">
              <Label for="lastName">Last Name</Label>
              <Input id="lastName" name="lastName" placeholder="Last Name" onChange={lastNameOnChange} value={lastName}/>
            </FormGroup>
            <FormGroup>
              <Label for="age">Age</Label>
              <Input type="number" id="age" name="age" placeholder="Age" onChange={ageOnChange} value={age}/>
            </FormGroup>
          </ModalBody>
          <ModalFooter>
            <Button color="primary" type="submit">
              Edit
            </Button>
            <Button color="secondary" onClick={toggleModal}>
              Cancel
            </Button>
          </ModalFooter>
        </Form>
      </Modal>
    </div>
  );
};
