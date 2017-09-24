/* global __api__ */
import VueAxios from 'vue-axios'
import Axios from 'axios'
import Vue from 'vue'
import { getAccessToken } from './auth'

const host = __api__ + '/api/'

Vue.use(VueAxios, Axios)

export default async function (query) {
  if (query.startsWith('#')) {
    query = defaultQueries[query.substring(1)]
  }

  var escapedQuery = query
    .replace(/"/g, '\\"')
  let queryString = '{query:"' + escapedQuery + '"}'
  var headers = { headers: { Authorization: `Bearer ${getAccessToken()}` } }
  let ret = await Axios.post(host, queryString, headers)
  return ret.data.data
}

const defaultQueries = {
  updateFileChanges: 'mutation{updateFileChanges{clientMutationId}}',
  allBooks: 'query{allBooks{title author id state bookState coverImage{id} path encodedPath}}',
  settings: 'query{settings{autoMatchThreshold path}}'
}
