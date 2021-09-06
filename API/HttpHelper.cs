﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    /// <summary>
    /// Вспомогательный код из п.1 подсказок к выполнению задания:
    /// https://gist.github.com/MaximKsh/acd77fc0352361a1fed64d146a694cf5
    /// </summary>

    /// <summary>
    /// Класс предназначен для хранения информации о результате HTTP запроса.
    /// </summary>
    public class WebServiceResponse
    {
        /// <summary>
        /// HTTP код ответа
        /// </summary>
        public HttpStatusCode StatusCode => this.HttpResponse.StatusCode;

        /// <summary>
        /// Результат выполнения HTTP запроса
        /// </summary>
        public HttpResponseMessage HttpResponse { get; set; }
    }

    /// <summary>
    /// Класс предназначен для хранения типизированной информации о результате HTTP запроса.
    /// </summary>
    /// <typeparam name="T">Тип объекта в HTTP-ответе.</typeparam>
    public class WebServiceResponse<T> : WebServiceResponse
    {
        /// <summary>
        /// Объект из ответа на запрос.
        /// </summary>
        public T Result { get; set; }
    }

    /// <summary>
    /// Вспомогательные функции для работы с HTTP
    /// </summary>
    public static class HttpHelper
    {
        /// <summary>
        /// Выполнить GET запрос.
        /// </summary>
        /// <param name="client">Объект HttpClient</param>
        /// <param name="url">Адрес, по которому выполняется запрос</param>
        /// <returns></returns>
        public static async Task<WebServiceResponse> RequestGetAsync(
            HttpClient client,
            string url)
        {
            // Выполнение запроса с помощью HttpClient
            var response = await client.GetAsync(url);

            // Возвращение результата
            return new WebServiceResponse
            {
                HttpResponse = response
            };
        }

        /// <summary>
        /// Выполнить GET запрос и получить типизированный результат
        /// </summary>
        /// <param name="client">Объект HttpClient</param>
        /// <param name="url">Адрес, по которому выполняется запрос</param>
        /// <typeparam name="T">Тип объекта в ответе</typeparam>
        /// <returns></returns>
        public static async Task<WebServiceResponse<T>> RequestGetAsync<T>(
            HttpClient client,
            string url) where T : class
        {
            // Выполнение запроса с помощью HttpClient
            var response = await client.GetAsync(url);

            // Если ответ не пуст, то выполняется десериализация содержимого из JSON в C# объект
            T result = null;
            if (response.Content.Headers.ContentLength != 0)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<T>(responseString);
            }

            // Возвращение результата
            return new WebServiceResponse<T>
            {
                Result = result,
                HttpResponse = response
            };
        }

        /// <summary>
        /// Выполнить POST запрос.
        /// </summary>
        /// <param name="client">Объект HttpClient</param>
        /// <param name="url">Адрес, по которому выполняется запрос</param>
        /// <param name="payloadObjects">Объекты, которые необходимо поместить в запрос</param>
        /// <returns></returns>
        public static async Task<WebServiceResponse> RequestPostAsync(
            HttpClient client,
            string url,
            params object[] payloadObjects)
        {
            // Необходимо правильно определить нагрузку запроса - это одиночный объект {...},
            // список объектов [{...}, {...}], либо объектов вообще нет и тело POST запроса должно быть пустым
            object payload;
            switch (payloadObjects.Length)
            {
                case 0:
                    payload = string.Empty;
                    break;
                case 1:
                    payload = payloadObjects[0];
                    break;
                default:
                    payload = payloadObjects;
                    break;
            }

            // Выполнение сериализации из C# объекта в JSON
            var payloadString = JsonConvert.SerializeObject(payload);

            // Выполнение кодирования.
            var payloadBytes = Encoding.UTF8.GetBytes(payloadString);
            var content = new ByteArrayContent(payloadBytes);

            // Установка служебной информации в HTTP заголовки.
            // Это необходимо для того, чтобы сервер мог корректно интерпретировать содержимое запроса
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            content.Headers.ContentEncoding.Add("UTF-8");
            content.Headers.ContentLength = payloadBytes.Length;

            // Выполняем POST запрос
            var response = await client.PostAsync(url, content);

            // Возвращаем результат
            return new WebServiceResponse
            {
                HttpResponse = response
            };
        }

        /// <summary>
        /// Выполнить POST запрос и получить типизированный результат
        /// </summary>
        /// <param name="client">Объект HttpClient</param>
        /// <param name="url">Адрес, по которому выполняется запрос</param>
        /// <param name="payloadObjects">Объекты, которые необходимо поместить в запрос</param>
        /// <typeparam name="T">Тип объекта в ответе</typeparam>
        /// <returns></returns>
        public static async Task<WebServiceResponse<T>> RequestPostAsync<T>(
            HttpClient client,
            string url,
            params object[] payloadObjects) where T : class
        {
            // Во избежание дублирования кода воспользуемся похожим методом
            var rawResponse = await RequestPostAsync(client, url, payloadObjects);
            var response = rawResponse.HttpResponse;

            // Если ответ не пуст, то выполняется десериализация содержимого из JSON в C# объект
            T result = null;
            if (response.Content.Headers.ContentLength != 0)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<T>(responseString);
            }

            // Возвращение результата
            return new WebServiceResponse<T>
            {
                Result = result,
                HttpResponse = response
            };
        }
    }

}