import React, { useState, useEffect } from 'react';
import { View, TextInput, Button, Alert, StyleSheet } from 'react-native';
import userApi from '../../../apis/userApi';

const UserManage = ({ navigation, route }) => {
  const currentDate = new Date(new Date().setFullYear(new Date().getFullYear() - 18));
  const user = route.params?.user;

  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [dateOfBirth, setDateOfBirth] = useState(currentDate);
  const [email, setEmail] = useState('');
  const [phoneNumber, setPhoneNumber] = useState('');
  const [role, setRole] = useState('USER');
  const [password, setPassword] = useState('');
  const [passwordConfirmation, setPasswordConfirmation] = useState('');
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (user) {
      setFirstName(user.firstName);
      setLastName(user.lastName);
      setDateOfBirth(new Date(user.dateOfBirth));
      setEmail(user.email);
      setPhoneNumber(user.phoneNumber);
      setRole(user.role);
      setPassword(user.password);
    }
  }, [user]);

  const handleSubmit = async () => {
    setLoading(true);
    try {
      if (user) {
        await userApi.put(`/users/${user.id}`, { id: user.id, firstName, lastName, dateOfBirth, email, phoneNumber, role, password, passwordConfirmation });
        Alert.alert('Success', 'User updated successfully.');
      } else {
        await userApi.post('/users', { firstName, lastName, dateOfBirth, email, phoneNumber, role, password, passwordConfirmation });
        Alert.alert('Success', 'User created successfully.');
      }
      navigation.goBack();
    } catch (error) {
      console.error(error);
      Alert.alert('Error', 'Failed to save user.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <View style={styles.container}>
      <TextInput
        style={styles.input}
        placeholder="First Name"
        value={firstName}
        onChangeText={setFirstName}
      />
      <TextInput
        style={styles.input}
        placeholder="Last Name"
        value={lastName}
        onChangeText={setLastName}
      />
      <TextInput
        style={styles.input}
        placeholder="Date of Birth"
        value={dateOfBirth.toISOString().split('T')[0]}
        onChangeText={setDateOfBirth}
      />
      <TextInput
        style={styles.input}
        placeholder="Email"
        value={email}
        onChangeText={setEmail}
        keyboardType="email-address"
      />
      <TextInput
        style={styles.input}
        placeholder="Phone Number"
        value={phoneNumber}
        onChangeText={setPhoneNumber}
      />
      <View style={styles.input}>
        <Picker
          selectedValue={role}
          onValueChange={(itemValue) => setRole(itemValue)}
        >
          <Picker.Item label="Admin" value="ADMIN" />
          <Picker.Item label="Moderator" value="MODERATOR" />
          <Picker.Item label="User" value="USER" />
        </Picker>
      </View>
      {!user && (
        <>
          <TextInput
            style={styles.input}
            placeholder="Password"
            value={password}
            onChangeText={setPassword}
            secureTextEntry
          />
          <TextInput
            style={styles.input}
            placeholder="Confirm Password"
            value={passwordConfirmation}
            onChangeText={setPasswordConfirmation}
            secureTextEntry
          />
        </>
      )}
      <Button
        title={loading ? 'Saving...' : user ? 'Update User' : 'Create User'}
        onPress={handleSubmit}
        disabled={loading}
      />
    </View>
  );
};

const styles = StyleSheet.create({
  container: { flex: 1, padding: 16, backgroundColor: '#fff' },
  input: { height: 50, borderWidth: 1, borderColor: '#ddd', borderRadius: 4, paddingHorizontal: 8, marginBottom: 16 },
});

export default UserManage;
