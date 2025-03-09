import axios from "axios";
import { Subscription } from "../types/types/order/Subscription";

const API_URL = "/api/subscriptions";

const subscriptionService = {
  /**
   * Fetch all subscriptions.
   */
  async getAll(): Promise<Subscription[]> {
    try {
      const response = await axios.get<Subscription[]>(API_URL);
      return response.data;
    } catch (error) {
      console.error("Error while fetching subscriptions:", error);
      throw error;
    }
  },

  /**
   * Fetch a subscription by its ID.
   */
  async getById(id: string): Promise<Subscription> {
    try {
      const response = await axios.get<Subscription>(`${API_URL}/${id}`);
      return response.data;
    } catch (error) {
      console.error("Error while fetching subscription:", error);
      throw error;
    }
  },

  /**
   * Create a new subscription.
   */
  async create(data: Partial<Subscription>): Promise<Subscription> {
    try {
      const response = await axios.post<Subscription>(API_URL, data);
      return response.data;
    } catch (error) {
      console.error("Error while creating subscription:", error);
      throw error;
    }
  },
};

export default subscriptionService;
