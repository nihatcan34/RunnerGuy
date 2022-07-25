using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    #region "LayerMask ile ilgili extension metodlar"
    public static class LayerMaskExtensionMethods
    {
        /// <summary>
        /// verilen layeri int degerine döndürür
        /// </summary>
        /// <param name="layerMask"></param>
        /// <returns></returns>
        public static int LayerMask2Int(LayerMask layerMask)
        {
            return (int)Mathf.Log(layerMask.value, 2);
        }
    }
    #endregion

