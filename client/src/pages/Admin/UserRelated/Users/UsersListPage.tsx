import React, { useEffect, useState } from 'react';
import { View, FlatList, Text, TouchableOpacity, ActivityIndicator, Alert, StyleSheet } from 'react-native';
import userApi from '../../../../apis/userApi';

const UsersList = ({ navigation }) => {
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetchUsers();
  }, []);

  const fetchUsers = async () => {
    try {
      const response = await userApi.get('/users');
      setUsers(response.data);
    } catch (error) {
      console.error(error);
      Alert.alert('Error', 'Failed to fetch users.');
    } finally {
      setLoading(false);
    }
  };

  const handleDeleteUser = async (userId) => {
    try {
      await userApi.delete(`/users/${userId}`);
      setUsers(users.filter((user) => user.id !== userId));
      Alert.alert('Success', 'User deleted successfully.');
    } catch (error) {
      console.error(error);
      Alert.alert('Error', 'Failed to delete user.');
    }
  };

  const renderUser = ({ item }) => (
    <View style={styles.userContainer}>
      <Text style={styles.userName}>{item.name}</Text>
      <View style={styles.actions}>
        <TouchableOpacity
          style={styles.actionButton}
          onPress={() => navigation.navigate('UserManage', { user: item })}>
          <Text style={styles.actionText}>Edit</Text>
        </TouchableOpacity>
        <TouchableOpacity
          style={[styles.actionButton, styles.deleteButton]}
          onPress={() => handleDeleteUser(item.id)}>
          <Text style={styles.actionText}>Delete</Text>
        </TouchableOpacity>
      </View>
    </View>
  );

  if (loading)
    return <ActivityIndicator size="large" color="#000" style={styles.loader} />;

  return (
    <View style={styles.container}>
      <TouchableOpacity
        style={styles.addButton}
        onPress={() => navigation.navigate('UserManage')}>
        <Text style={styles.addButtonText}>Add User</Text>
      </TouchableOpacity>
      <FlatList
        data={users}
        keyExtractor={(item) => item.id.toString()}
        renderItem={renderUser}
      />
    </View>
  );
};

const styles = StyleSheet.create({
  container: { flex: 1, padding: 16, backgroundColor: '#fff' },
  loader: { flex: 1, justifyContent: 'center', alignItems: 'center' },
  userContainer: { padding: 16, borderBottomWidth: 1, borderColor: '#ddd' },
  userName: { fontSize: 18, marginBottom: 8 },
  actions: { flexDirection: 'row', justifyContent: 'space-between' },
  actionButton: { padding: 8, borderWidth: 1, borderColor: '#007bff', borderRadius: 4 },
  deleteButton: { borderColor: '#dc3545' },
  actionText: { color: '#007bff' },
  addButton: { padding: 16, backgroundColor: '#007bff', borderRadius: 4, marginBottom: 16 },
  addButtonText: { color: '#fff', textAlign: 'center', fontWeight: 'bold' },
});

export default UsersList;
