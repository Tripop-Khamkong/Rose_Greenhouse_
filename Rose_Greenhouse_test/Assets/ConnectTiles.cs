using UnityEngine;

public class ConnectTiles : MonoBehaviour {
public enum Side { Top, Right, Bottom, Left };
public void ConnectTo(GameObject otherTile, Side thisSide) {
    Side otherSide = (Side)(((int)thisSide + 2) % 4);
    Transform thisTransform = transform.Find(thisSide.ToString());
    Transform otherTransform = otherTile.transform.Find(otherSide.ToString());
    thisTransform.position = (thisTransform.position + otherTransform.position) / 2;
    thisTransform.right = otherTransform.position - thisTransform.position;
    
}
}

