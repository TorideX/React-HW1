import React, { useState } from "react";
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

export const AddDeveloperModal = ({
  isModalOpen,
  toggleModal,
  submitCallback,
}) => {
  return (
    <div>
      <Modal isOpen={isModalOpen} toggle={toggleModal}>
        <ModalHeader toggle={toggleModal}>Add Developer Page</ModalHeader>
        <Form onSubmit={submitCallback}>
          <ModalBody>
            <FormGroup className="mb-4">
              <Label for="firstName">First Name</Label>
              <Input id="firstName" name="firstName" placeholder="First Name" />
            </FormGroup>
            <FormGroup className="mb-4">
              <Label for="lastName">Last Name</Label>
              <Input id="lastName" name="lastName" placeholder="Last Name" />
            </FormGroup>
            <FormGroup>
              <Label for="age">Age</Label>
              <Input type="number" id="age" name="age" placeholder="Age" />
            </FormGroup>
          </ModalBody>
        <ModalFooter>
          <Button color="primary" type='submit'>
            Add
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
